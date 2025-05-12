using Microsoft.Data.SqlClient;
using TestApbdGroupA.Models;

namespace TestApbdGroupA.Reopisotries;

public class ApoinmentRepository : IApoinmentRepository
{
    private readonly string _connectionString;

    public ApoinmentRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    
    public async Task<ApoinmentDto> GetApoinmnetsDetailsAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand(@"
            SELECT v.date, c.first_name, c.last_name, c.date_of_birth,
                   m.mechanic_id, m.licence_number,
                   s.name, vs.service_fee
            FROM Visit v
            JOIN Client c ON v.client_id = c.client_id
            JOIN Mechanic m ON v.mechanic_id = m.mechanic_id
            JOIN Visit_Service vs ON v.visit_id = vs.visit_id
            JOIN Service s ON vs.service_id = s.service_id
            WHERE v.visit_id = @id", connection);

        command.Parameters.AddWithValue("@id", id);

        using var reader = await command.ExecuteReaderAsync();

        if (!reader.HasRows) return null;

        ApoinmentDto result = null;
        var services = new List<AppointmentServicesDto>();

        while (await reader.ReadAsync())
        {
            if (result == null)
            {
                result = new ApoinmentDto
                {
                    Date = reader.GetDateTime(0),
                    Patient= new PatientDto
                    {
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        DateOfBirth = reader.GetDateTime(3)
                    },
                    Doctor = new DoctorDto
                    {
                        Id= reader.GetInt32(4),
                        Pwz = reader.GetString(5)
                    },
                    Services = services
                };
            }

            services.Add(new AppointmentServicesDto
            {
                Name = reader.GetString(6),
                ServiceFee = reader.GetDecimal(7)
            });
        }

        return result;
    }

}