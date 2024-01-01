using System;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = "Server=localhost; Database=BDProject; User Id=sa; Password=20186947Ismael";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            Console.WriteLine("Conexión exitosa");

            string salir = "N", tipoDocumento, documento, nombres, apellidos, fechaNacimiento, sexo, fechaIngreso;
            decimal sueldo;

            while (salir == "N")
            {
                Console.Write("Tipo de Documento: ");
                tipoDocumento = Console.ReadLine();
                Console.Write("Documento: ");
                documento = Console.ReadLine();
                Console.Write("Nombres: ");
                nombres = Console.ReadLine();
                Console.Write("Apellidos: ");
                apellidos = Console.ReadLine();
                Console.Write("Fecha de Nacimiento (YYYY-MM-DD): ");
                fechaNacimiento = Console.ReadLine();
                Console.Write("Sexo: ");
                sexo = Console.ReadLine();
                Console.Write("Fecha de Ingreso (YYYY-MM-DD): ");
                fechaIngreso = Console.ReadLine();
                Console.Write("Sueldo: ");
                sueldo = Convert.ToDecimal(Console.ReadLine());

                // Usar el procedimiento almacenado para realizar upsert en la tabla de clientes
                using (SqlCommand upsertClienteCommand = new SqlCommand("UpsertCliente", connection))
                {
                    upsertClienteCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    upsertClienteCommand.Parameters.Add(new SqlParameter("@TipoDocumento", tipoDocumento));
                    upsertClienteCommand.Parameters.Add(new SqlParameter("@Documento", documento));
                    upsertClienteCommand.Parameters.Add(new SqlParameter("@Nombres", nombres));
                    upsertClienteCommand.Parameters.Add(new SqlParameter("@Apellidos", apellidos));
                    upsertClienteCommand.Parameters.Add(new SqlParameter("@FechaNacimiento", fechaNacimiento));
                    upsertClienteCommand.Parameters.Add(new SqlParameter("@Sexo", sexo));
                    upsertClienteCommand.Parameters.Add(new SqlParameter("@FechaIngreso", fechaIngreso));
                    upsertClienteCommand.Parameters.Add(new SqlParameter("@Sueldo", sueldo));

                    upsertClienteCommand.ExecuteNonQuery();
                    Console.WriteLine("Datos de cliente insertados/actualizados exitosamente");

                    // Ahora, insertar en la tabla de nómina
                    Console.Write("Tipo de Pago: ");
                    string tipoPago = Console.ReadLine();
                    Console.Write("Monto: ");
                    decimal monto = Convert.ToDecimal(Console.ReadLine());
                    Console.Write("Fecha de Pago (YYYY-MM-DD): ");
                    string fechaPago = Console.ReadLine();
                    Console.Write("Mes: ");
                    int mes = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Año: ");
                    int ano = Convert.ToInt32(Console.ReadLine());

                    // Usar el procedimiento almacenado para insertar datos en la tabla de nómina
                    using (SqlCommand insertNominaCommand = new SqlCommand("InsertNomina", connection))
                    {
                        insertNominaCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        insertNominaCommand.Parameters.Add(new SqlParameter("@TipoDocumento", tipoDocumento));
                        insertNominaCommand.Parameters.Add(new SqlParameter("@Documento", documento));
                        insertNominaCommand.Parameters.Add(new SqlParameter("@TipoPago", tipoPago));
                        insertNominaCommand.Parameters.Add(new SqlParameter("@Monto", monto));
                        insertNominaCommand.Parameters.Add(new SqlParameter("@FechaPago", fechaPago));
                        insertNominaCommand.Parameters.Add(new SqlParameter("@Mes", mes));
                        insertNominaCommand.Parameters.Add(new SqlParameter("@Año", ano));

                        insertNominaCommand.ExecuteNonQuery();
                        Console.WriteLine("Datos de nómina insertados exitosamente");
                    }

                    Console.WriteLine("¿Desea salir? (Y/N)");
                    salir = Console.ReadLine().ToUpper();
                }
            }
        }
    }
}
