using grupo4FarmaceuticaProyectoMov.Models;
using grupo4FarmaceuticaProyectoMov.Utils;
using System.Text.Json;
using System.Text;

namespace grupo4FarmaceuticaProyectoMov.Views;

public partial class vActualizarEliminar : ContentView
{
    
    private Usuario usuarioActualizar { get; set; }
    public MedicamentoDto Medicamento { get; set; }

    public vActualizarEliminar(MedicamentoDto medicamento, Usuario usuario)
    {
        InitializeComponent();
        // Deshabilitar la flecha de retroceso en la página de Login
        NavigationPage.SetHasBackButton(this, false);
        usuarioActualizar = usuario;
        Medicamento = medicamento;

        lblBienvenido.Text = $"Bienvenido: {usuarioActualizar.nombre} {usuarioActualizar.apellido}";
        lblIdMedicamento.Text = Medicamento.id.ToString(); // No visible, solo para referencia
        txtNombreMedicamento.Text = Medicamento.nombreMedicamento;
        txtPresentacion.Text = Medicamento.presentacion;
        txtIndicaciones.Text = Medicamento.indicaciones;
        datepickerFechaExpiracion.Date = Medicamento.fechaExpiracion;
        txtUbicacionFarmacia.Text = Medicamento.ubicacionFarmacia;
        txtPrecio.Text = Medicamento.precio.ToString();
     }
        
    

    private async void btnActualizar_Clicked(object sender, EventArgs e)
    {
        // Validar nombre del medicamento
        if (string.IsNullOrWhiteSpace(txtNombreMedicamento.Text))
        {
            await DisplayAlert("Error", "Ingresa el nombre del medicamento.", "Cerrar");
            return;
        }

        // Validar presentación
        if (string.IsNullOrWhiteSpace(txtPresentacion.Text))
        {
            await DisplayAlert("Error", "Ingresa la presentación del medicamento.", "Cerrar");
            return;
        }

        // Validar indicaciones
        if (string.IsNullOrWhiteSpace(txtIndicaciones.Text))
        {
            await DisplayAlert("Error", "Ingresa las indicaciones del medicamento.", "Cerrar");
            return;
        }

        // Validar fecha de expiración
        if (datepickerFechaExpiracion.Date <= DateTime.Now)
        {
            await DisplayAlert("Error", "La fecha de expiración debe ser posterior a la fecha actual.", "Cerrar");
            return;
        }

        // Validar ubicación en la farmacia
        if (string.IsNullOrWhiteSpace(txtUbicacionFarmacia.Text))
        {
            await DisplayAlert("Error", "Ingresa la ubicación del medicamento en la farmacia.", "Cerrar");
            return;
        }

        // Validar precio
        if (string.IsNullOrWhiteSpace(txtPrecio.Text) || !decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
        {
            await DisplayAlert("Error", "Ingresa un precio válido para el medicamento. mayor a 0.00", "Cerrar");
            return;
        }

        // Crear una instancia de MedicamentoDto con los valores del formulario
        var medicamentoDto = new MedicamentoDto
        {
            id = Medicamento.id, // Mantener el mismo ID para la actualización
            nombreMedicamento = txtNombreMedicamento.Text,
            presentacion = txtPresentacion.Text,
            indicaciones = txtIndicaciones.Text,
            fechaExpiracion = datepickerFechaExpiracion.Date,
            ubicacionFarmacia = txtUbicacionFarmacia.Text,
            precio = decimal.Parse(txtPrecio.Text)
        };

        // Convertir el objeto a JSON
        var jsonContent = JsonSerializer.Serialize(medicamentoDto);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Crear una instancia de HttpClient y realizar la solicitud POST para actualizar el medicamento
        using (var client = new HttpClient())
        {
            try
            {
                // URL del endpoint de actualización con el ID del medicamento
                var url = $"http://{Config.LocalMachineIp}:8080/medicamento/actualizar/{medicamentoDto.id}";

                // Enviar la solicitud POST
                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Si la solicitud fue exitosa, puedes mostrar un mensaje y redirigir
                    await DisplayAlert("Éxito", "Medicamento actualizado exitosamente.", "OK");
                    var principalMedicamento = new PrincipalMedicamento(usuarioActualizar);
                    await Navigation.PushAsync(principalMedicamento);
                }
                else
                {
                    // Si hubo un error, muestra un mensaje con el código de estado
                    await DisplayAlert("Error", "Hubo un problema al actualizar el medicamento.", "OK");
                }
            }
            catch (Exception ex)
            {
                // Manejar errores de conexión o problemas de red
                await DisplayAlert("Error", $"Error al conectar con el servidor: {ex.Message}", "OK");
            }
        }
    }

    private async void datepickerFechaExpiracion_DateSelected(object sender, DateChangedEventArgs e)
    {
        // Lógica cuando la fecha cambia
        lblFechaSeleccionada.Text = $"Fecha seleccionada: {e.NewDate:dd/MM/yyyy}";
    }


    private async void btnSalir_Clicked(object sender, EventArgs e)
    {
      var opciones = new Opciones(usuarioActualizar);
        await Navigation.PushAsync(opciones);
    }

    private async void btnEliminar_Clicked(object sender, EventArgs e)
    {
        // Confirmar si el usuario realmente quiere eliminar el medicamento
        bool confirmacion = await DisplayAlert("Confirmar Eliminación", "¿Estás seguro de que deseas eliminar este medicamento?", "Sí", "No");

        if (confirmacion)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // URL del endpoint de eliminación
                    var url = $"http://{Config.LocalMachineIp}:8080/medicamento/eliminar/{Medicamento.id}";

                    // Realizar la solicitud POST para eliminar el medicamento
                    var response = await client.PostAsync(url, null); // Pasar 'null' como contenido ya que no hay cuerpo en la solicitud

                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Éxito", "Medicamento eliminado correctamente.", "OK");
                        var principalMedicamento = new PrincipalMedicamento(usuarioActualizar);
                        await Navigation.PushAsync(principalMedicamento);
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo eliminar el medicamento.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Hubo un problema al eliminar el medicamento: {ex.Message}", "OK");
            }
        }
    }

}
    
