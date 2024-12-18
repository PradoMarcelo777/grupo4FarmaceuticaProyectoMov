using grupo4FarmaceuticaProyectoMov.Models;
using grupo4FarmaceuticaProyectoMov.Utils;
using System.Text;
using System.Text.Json;

namespace grupo4FarmaceuticaProyectoMov.Views;

public partial class vRegistro : ContentPage
{
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; }
    public string ApellidoUsuario { get; set; }
    public Usuario usuarioRegistroConsulta { get; set; }

    public vRegistro(Usuario usuario)
	{
		InitializeComponent();
        IdUsuario = usuario.id;
        NombreUsuario = usuario.nombre;
        ApellidoUsuario = usuario.apellido;
        usuarioRegistroConsulta = usuario;

        lblBienvenido.Text = $"Bienvenido: {NombreUsuario} {ApellidoUsuario}";

        // Deshabilitar la flecha de retroceso en la p?gina de Login
        NavigationPage.SetHasBackButton(this, false);

        // Llamar al m?todo para obtener las consultas
        ObtenerConsultas();
    }


    private async void ObtenerConsultas()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                // URL de la API para obtener las consultas
                var url = $"http://{Config.LocalMachineIp}:8080/registro/obtener/{IdUsuario}";

                // Enviar la solicitud POST (ajustar si es necesario enviar un cuerpo JSON)
                var content = new StringContent("{}", Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Leer la respuesta como string
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserializar el JSON en una lista de consultas
                    var consultas = JsonSerializer.Deserialize<List<RegistroConsultas>>(jsonResponse);

                    // Asignar la lista de consultas al ItemsSource de la ListView
                    listaConsultas.ItemsSource = consultas;
                }
                else
                {
                    await DisplayAlert("Error", "No se pudieron obtener las consultas", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar errores de solicitud
            await DisplayAlert("Error", $"Hubo un problema al obtener las consultas: {ex.Message}", "OK");
        }
    }

    private async void listaConsultas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        // Verifica si se ha seleccionado un item y lo obtiene
        if (e.SelectedItem != null)
        {
            // Obtiene el objeto seleccionado (RegistroConsultas)
            var consultaSeleccionada = (RegistroConsultas)e.SelectedItem;

        }
    }

    private async void btnSalir_Clicked(object sender, EventArgs e)
    {
        // Eliminar cualquier valor almacenado en las Preferences
        Preferences.Remove("username");
        Preferences.Remove("password");

        var opciones = new Opciones(usuarioRegistroConsulta);
        await Navigation.PushAsync(opciones);
    }
}