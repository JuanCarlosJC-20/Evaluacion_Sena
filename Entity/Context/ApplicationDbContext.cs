using Microsoft.EntityFrameworkCore;
using Entity.Model;
using Entity.Model.Base;
using System.Reflection;

namespace Entity.Context
{
    /// <summary>
    /// Contexto de base de datos para el sistema de gestión médica.
    /// Maneja las entidades principales: Pacientes, Doctores y Citas Médicas.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor del contexto que recibe las opciones de configuración
        /// </summary>
        /// <param name="options">Opciones de configuración del contexto</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// DbSet para gestionar las entidades de Pacientes
        /// </summary>
        public DbSet<Patient> Patients { get; set; }

        /// <summary>
        /// DbSet para gestionar las entidades de Doctores
        /// </summary>
        public DbSet<Doctor> Doctors { get; set; }

        /// <summary>
        /// DbSet para gestionar las entidades de Citas Médicas
        /// </summary>
        public DbSet<Appointment> Appointments { get; set; }

        /// <summary>
        /// Configura el modelo de datos y las relaciones entre entidades
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar configuraciones automáticamente desde el assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Configuración global para todas las entidades que hereden de BaseEntity
            ConfigureBaseEntityProperties(modelBuilder);

            // Configuración específica de Patient
            ConfigurePatient(modelBuilder);

            // Configuración específica de Doctor
            ConfigureDoctor(modelBuilder);

            // Configuración específica de Appointment
            ConfigureAppointment(modelBuilder);

            // Configurar relaciones
            ConfigureRelationships(modelBuilder);
        }

        /// <summary>
        /// Configura las propiedades comunes para todas las entidades que heredan de BaseEntity
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo</param>
        private void ConfigureBaseEntityProperties(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    // Configurar propiedades comunes de BaseEntity
                    modelBuilder.Entity(entityType.ClrType)
                        .Property("Id")
                        .IsRequired();

                    modelBuilder.Entity(entityType.ClrType)
                        .Property("CreatedAt")
                        .IsRequired()
                        .HasDefaultValueSql("GETDATE()");

                    modelBuilder.Entity(entityType.ClrType)
                        .Property("UpdatedAt")
                        .IsRequired()
                        .HasDefaultValueSql("GETDATE()");

                    modelBuilder.Entity(entityType.ClrType)
                        .Property("Status")
                        .IsRequired()
                        .HasDefaultValue(true);

                    // Índice en Status para optimizar consultas de entidades activas
                    modelBuilder.Entity(entityType.ClrType)
                        .HasIndex("Status")
                        .HasDatabaseName($"IX_{entityType.ClrType.Name}_Status");
                }
            }
        }

        /// <summary>
        /// Configuración específica para la entidad Patient
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo</param>
        private void ConfigurePatient(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patients");

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(p => p.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(p => p.Phone)
                    .IsRequired();

                entity.Property(p => p.DNI)
                    .IsRequired();

                // Índices para optimización
                entity.HasIndex(p => p.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_Patients_Email");

                entity.HasIndex(p => p.DNI)
                    .IsUnique()
                    .HasDatabaseName("IX_Patients_DNI");

                entity.HasIndex(p => p.Name)
                    .HasDatabaseName("IX_Patients_Name");
            });
        }

        /// <summary>
        /// Configuración específica para la entidad Doctor
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo</param>
        private void ConfigureDoctor(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("Doctors");

                entity.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(d => d.Specialty)
                    .IsRequired()
                    .HasMaxLength(100);

                // Índices para optimización
                entity.HasIndex(d => d.Name)
                    .HasDatabaseName("IX_Doctors_Name");

                entity.HasIndex(d => d.Specialty)
                    .HasDatabaseName("IX_Doctors_Specialty");
            });
        }

        /// <summary>
        /// Configuración específica para la entidad Appointment
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo</param>
        private void ConfigureAppointment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointments");

                entity.Property(a => a.Date)
                    .IsRequired();

                entity.Property(a => a.Reason)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(a => a.PatientId)
                    .IsRequired();

                entity.Property(a => a.DoctorId)
                    .IsRequired();

                // Índices para optimización
                entity.HasIndex(a => a.Date)
                    .HasDatabaseName("IX_Appointments_Date");

                entity.HasIndex(a => a.PatientId)
                    .HasDatabaseName("IX_Appointments_PatientId");

                entity.HasIndex(a => a.DoctorId)
                    .HasDatabaseName("IX_Appointments_DoctorId");

                // Índice compuesto para evitar citas duplicadas (opcional)
                entity.HasIndex(a => new { a.DoctorId, a.Date })
                    .HasDatabaseName("IX_Appointments_DoctorDate");
            });
        }

        /// <summary>
        /// Configura las relaciones entre entidades
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo</param>
        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // Relación Appointment -> Patient (Muchas citas pueden tener un paciente)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Appointments_Patients");

            // Relación Appointment -> Doctor (Muchas citas pueden tener un doctor)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Appointments_Doctors");
        }

        /// <summary>
        /// Override del método SaveChanges para manejar automáticamente la auditoría
        /// </summary>
        /// <returns>Número de entidades afectadas</returns>
        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        /// <summary>
        /// Override del método SaveChangesAsync para manejar automáticamente la auditoría
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Número de entidades afectadas</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Actualiza automáticamente los campos de auditoría para todas las entidades modificadas
        /// </summary>
        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                    // Asegurar que CreatedAt no se modifique en updates
                    entry.Property(nameof(BaseEntity.CreatedAt)).IsModified = false;
                }
            }
        }
    }
}
