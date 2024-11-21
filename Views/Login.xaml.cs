using grupo4FarmaceuticaProyectoMov.Utils;
using System.Text;
using grupo4FarmaceuticaProyectoMov.Models;
using System.Text;


namespace grupo4FarmaceuticaProyectoMov.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
        // Deshabilitar la flecha de retroceso en la p�gina de Login
        NavigationPage.SetHasBackButton(this, false);
    }

    private async void btnIniciarSesion_Clicked(object sender, EventArgs e)
    {
        // Validar nombre de usuario
        if (string.IsNullOrWhiteSpace(txtUsuario.Text))
        {
            await DisplayAlert("Error", "Ingresa tu nombre de usuario.", "Cerrar");
            return;
        }

        // Validar contrase�a
        if (string.IsNullOrWhiteSpace(txtContrasena.Text))
        {
            await DisplayAlert("Error", "Ingresa tu contrase�a.", "Cerrar");
            return;
        }

        // Obtener las credenciales del usuario
        string username = txtUsuario.Text;
        string password = txtContrasena.Text;

        // Crear el objeto con las credenciales para el login
        var loginData = new
        {
            username = username,
            password = password
        };

        try
        {
            // Crear el HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Establecer la URL del endpoint
                var url = $"http://{Config.LocalMachineIp}:8080/usuario/login";

                // Serializar el objeto loginData a JSON
                var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

                // Realizar la solicitud POST al endpoint de login
                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Leer la respuesta del servidor
                    var responseData = await response.Content.ReadAsStringAsync();

                    // Deserializar la respuesta JSON a un objeto Usuario
                    var usuario = JsonConvert.DeserializeObject<Usuario>(responseData);

                    // Aqu� ya tienes el objeto 'usuario' con los datos deserializados
                    // Puedes acceder al id, nombre, apellido, etc.

                    // Redirigir a la p�gina de opciones, pasando el objeto usuario
                    await Navigation.PushAsync(new Opciones(usuario));
                }
                else
                {
                    // Mostrar mensaje de error si el login falla
                    await DisplayAlert("Error", "Usuario o contrase�a incorrectos", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            // Mostrar un mensaje en caso de que ocurra un error al hacer la solicitud
            await DisplayAlert("Error", $"Ha ocurrido un error: {ex.Message}", "OK");
        }
    }

    private async void btnRegistrarse_Clicked(object sender, EventArgs e)
    {

        try
        {
            // Crear un objeto Usuario vac�o o con valores por defecto
            var usuario = new Usuario
            {
                Id = 0, // Si es un nuevo usuario, puedes poner 0 o un valor predeterminado
                nombre = "",
                apellido = "",
                username = "",
                password = "",
                email = ""
            };

            // Redirigir a la p�gina de registro (vUsuario) y pasar el objeto Usuario
            await Navigation.PushAsync(new vUsuarios(usuario));
            
        }
        catch (Exception ex)
        {
            // Manejar cualquier error
            await DisplayAlert("Error", $"Ha ocurrido un error al intentar redirigir: {ex.Message}", "OK");
        }


    }

}