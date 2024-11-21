namespace grupo4FarmaceuticaProyectoMov.Views;
using grupo4FarmaceuticaProyectoMov.Models;
using grupo4FarmaceuticaProyectoMov.Utils;
using Newtonsoft.Json;
using System.Text;

public partial class vUsuarios : ContentPage
{
    public List<Rol> Roles { get; set; } = new List<Rol>();
    private Usuario Usuario { get; set; } // Almacenar el objeto Usuario completo
    public vUsuarios(Usuario usuario)
	{
		InitializeComponent();
        // Deshabilitar la flecha de retroceso en la p�gina de Login
        NavigationPage.SetHasBackButton(this, false);

        // Asignamos el objeto usuario completo
        Usuario = usuario;

        lblBienvenido.Text = $"Bienvenido: {Usuario.nombre} {Usuario.apellido}";

        // Cargar roles en el Picker
        CargarRoles();
    }

    private async void CargarRoles()
    {
        try
        {
            // Obtener los roles desde la API
            Roles = await ObtenerRolesDesdeApi();

            // Enlazar la lista al Picker
            pickerRol.ItemsSource = Roles;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar los roles: {ex.Message}", "OK");
        }
    }


    private async Task<List<Rol>> ObtenerRolesDesdeApi()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                // Establecer la URL de la API
                var url = $"http://{Config.LocalMachineIp}:8080/rol/obtener";

                // Crear un cuerpo vac�o (si tu API requiere un POST con cuerpo vac�o)
                var content = new StringContent("{}", Encoding.UTF8, "application/json");

                // Realizar la solicitud POST
                var response = await client.PostAsync(url, content);

                // Verificar si la respuesta fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer la respuesta como string
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserializar el JSON a una lista de roles
                    return JsonConvert.DeserializeObject<List<Rol>>(jsonResponse);
                }
                else
                {
                    throw new Exception($"Error en la solicitud: {response.ReasonPhrase}");
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar errores
            throw new Exception($"Error al obtener los roles desde la API: {ex.Message}");
        }
    }

    private async void btnCrear_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Validar username
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                await DisplayAlert("Error", "Ingresa un nombre de usuario.", "OK");
                return;
            }

            // Validar contrase�a
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                await DisplayAlert("Error", "Ingresa una contrase�a.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text) || txtPassword.Text.Length < 6)
            {
                await DisplayAlert("Error", "La contrase�a es obligatoria y debe tener al menos 6 caracteres.", "OK");
                return;
            }

            // Validar nombre
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                await DisplayAlert("Error", "Ingresa un nombre.", "OK");
                return;
            }

            // Validar apellido
            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                await DisplayAlert("Error", "Ingresa un apellido.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                await DisplayAlert("Error", "Debe ingresar el correo electr�nico empresarial @farmaec.com valido.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                await DisplayAlert("Error", "Ingresa un correo electr�nico v�lido.", "OK");
                return;
            }

            // Validar rol
            if (pickerRol.SelectedItem == null)
            {
                await DisplayAlert("Error", "Debe seleccionar un rol.", "OK");
                return;
            }

            // Recoger los valores del formulario
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            string email = txtEmail.Text;

            // Obtener el rol seleccionado
            Rol rolSeleccionado = (Rol)pickerRol.SelectedItem;

            if (rolSeleccionado == null)
            {
                await DisplayAlert("Error", "Debe seleccionar un rol", "OK");
                return;
            }

            // Crear el objeto Usuario con el rol que solo tiene el Id
            var usuarioRequest = new Usuario
            {
                username = username,
                password = password,
                nombre = nombre,
                apellido = apellido,
                email = email,
                rol = new Rol { id = rolSeleccionado.id } // Solo enviar el Id del rol
            };

            // Convertir el objeto Usuario a JSON
            var json = JsonConvert.SerializeObject(usuarioRequest);

            // Enviar la solicitud POST a la API
            using (HttpClient client = new HttpClient())
            {
                var url = $"http://{Config.LocalMachineIp}:8080/usuario/crear"; // URL de la API de creaci�n de usuario

                // Crear el contenido de la solicitud
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Realizar la solicitud POST
                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("�xito", "Usuario creado con �xito", "OK");

                    await Navigation.PushAsync(new Login());
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"No se pudo crear el usuario: {errorMessage}", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Hubo un problema al crear el usuario: {ex.Message}", "OK");
        }


    }

    private async void btnSalir_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            // Validar el formato del correo electr�nico
            var addr = new System.Net.Mail.MailAddress(email);

            // Verificar que el dominio sea el esperado
            string dominioEsperado = "@farmaec.com";
            if (!email.EndsWith(dominioEsperado, StringComparison.OrdinalIgnoreCase))
            {
                return false; // El correo no tiene el dominio correcto
            }

            return addr.Address == email;
        }
        catch
        {
            return false; // El formato es inv�lido
        }
    }


}