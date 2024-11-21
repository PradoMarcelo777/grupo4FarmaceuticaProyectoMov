using grupo4FarmaceuticaProyectoMov.Models;
using grupo4FarmaceuticaProyectoMov.Utils;
using System.Text;
using System.Text.Json;
namespace grupo4FarmaceuticaProyectoMov.Views;

public partial class Medicamento : ContentPage
{
    private Usuario usuarioCrear { get; set; } 

    public Medicamento(Usuario usuario)
    {
        InitializeComponent();
        usuarioCrear = usuario;
        lblBienvenido.Text = $"Bienvenido: {usuarioCrear.nombre} {usuarioCrear.apellido}";
        
        NavigationPage.SetHasBackButton(this, false);
    }

    private async void btnCrearMedicamento_Clicked(object sender, EventArgs e)
    {
        
        if (string.IsNullOrWhiteSpace(txtNombreMedicamento.Text))
        {
            await DisplayAlert("Error", "Ingresa el nombre del medicamento.", "Cerrar");
            return;
        }

        
        if (string.IsNullOrWhiteSpace(txtPresentacion.Text))
        {
            await DisplayAlert("Error", "Ingresa la presentación del medicamento.", "Cerrar");
            return;
        }

        
        if (string.IsNullOrWhiteSpace(txtIndicaciones.Text))
        {
            await DisplayAlert("Error", "Ingresa las indicaciones del medicamento.", "Cerrar");
            return;
        }

        if (datepickerFechaExpiracion.Date <= DateTime.Now)
        {
            await DisplayAlert("Error", "La fecha de expiración debe ser posterior a la fecha actual.", "Cerrar");
            return;
        }

        
        if (string.IsNullOrWhiteSpace(txtUbicacionFarmacia.Text))
        {
            await DisplayAlert("Error", "Ingresa la ubicación del medicamento en la farmacia.", "Cerrar");
            return;
        }

        
        if (string.IsNullOrWhiteSpace(txtPrecio.Text) || !decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
        {
            await DisplayAlert("Error", "Ingresa un precio válido para el medicamento. mayor a 0.00", "Cerrar");
            return;
        }
        
        var medicamentoDto = new MedicamentoDto
        {
            nombreMedicamento = txtNombreMedicamento.Text,
            presentacion = txtPresentacion.Text,
            indicaciones = txtIndicaciones.Text,
            fechaExpiracion = datepickerFechaExpiracion.Date,
            ubicacionFarmacia = txtUbicacionFarmacia.Text,
            precio = decimal.Parse(txtPrecio.Text)
        };

        
        var jsonContent = JsonSerializer.Serialize(medicamentoDto);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        
        using (var client = new HttpClient())
        {
            try
            {
                
                var response = await client.PostAsync($"http://{Config.LocalMachineIp}:8080/medicamento/crear", content);

                if (response.IsSuccessStatusCode)
                {
                    
                    await DisplayAlert("Éxito", "Medicamento creado exitosamente.", "OK");
                    var principalMedicamento = new PrincipalMedicamento(usuarioCrear);
                    await Navigation.PushAsync(principalMedicamento);
                }
                else
                {
                    
                    await DisplayAlert("Error", "Hubo un problema al crear el medicamento.", "OK");
                }
            }
            catch (Exception ex)
            {
                
                await DisplayAlert("Error", $"Error al conectar con el servidor: {ex.Message}", "OK");
            }
        }

    }

    private void datepickerFechaExpiracion_DateSelected(object sender, DateChangedEventArgs e)
    {
        lblFechaSeleccionada.Text = $"Fecha seleccionada: {e.NewDate:yyyy-MM-dd}";
    }

    private async void btnSalir_Clicked(object sender, EventArgs e)
    {
        var opciones = new Opciones(usuarioCrear);
        await Navigation.PushAsync(opciones);
    }

}