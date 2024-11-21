using grupo4FarmaceuticaProyectoMov.Utils;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text;

namespace grupo4FarmaceuticaProyectoMov.Views;

public partial class PrincipalMedicamento : ContentView
{
    // Propiedades para almacenar los datos del usuario
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; }
    public string ApellidoUsuario { get; set; }

    public Usuario usuarioMedicamento { get; set; }

    public ObservableCollection<Medicamento> Medicamentos { get; set; }

    public PrincipalMedicamento(Usuario usuario)
    {
        InitializeComponent();
        // Asignar los valores a las propiedades desde el objeto usuario
        IdUsuario = usuario.id;
        NombreUsuario = usuario.nombre;
        ApellidoUsuario = usuario.apellido;
        usuarioMedicamento = usuario;

        // Aquí puedes utilizar estos datos para mostrar la información en la UI
        lblBienvenido.Text = $"Bienvenido: {NombreUsuario} {ApellidoUsuario}";

        // Deshabilitar la flecha de retroceso en la página de Login
        NavigationPage.SetHasBackButton(this, false);

        ObtenerMedicamentos();
    }

    private async void ObtenerMedicamentos()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                // URL de la API para obtener los medicamentos
                var url = $"http://{Config.LocalMachineIp}:8080/medicamento/obtener";

                // Enviar la solicitud POST (aquí puedes ajustar si no necesitas enviar un cuerpo JSON)
                var content = new StringContent("{}", Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Leer la respuesta como string
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    // Deserializar el JSON en una lista de medicamentos
                    var medicamentos = JsonSerializer.Deserialize<List<MedicamentoDto>>(jsonResponse);

                    // Asignar la lista de medicamentos al ItemsSource de la ListView
                    listaMedicamentos.ItemsSource = medicamentos;
                }
                else
                {
                    await DisplayAlert("Error", "No se pudieron obtener los medicamentos", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar errores de solicitud
            await DisplayAlert("Error", $"Hubo un problema al obtener los medicamentos: {ex.Message}", "OK");
        }
    }
    private void btnInsertar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Medicamento(usuarioMedicamento));
    }

    private async void btnSalir_Clicked(object sender, EventArgs e)
    {
        var opciones = new Opciones(usuarioMedicamento);
        await Navigation.PushAsync(opciones);
    }

    private async void listaMedicamentos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        {
            // Verifica si se ha seleccionado un item y lo obtiene
            if (e.SelectedItem != null)
            {
                // Obtiene el objeto seleccionado (MedicamentoDto)
                var medicamentoSeleccionado = (MedicamentoDto)e.SelectedItem;

                // Navegar a la página de actualización/eliminación y pasar el medicamento seleccionado
                await Navigation.PushAsync(new vActualizarEliminar(medicamentoSeleccionado, usuarioMedicamento));
            }
        }
    }
}
