using grupo4FarmaceuticaProyectoMov.Models;
using grupo4FarmaceuticaProyectoMov.Utils;
using System.Text.Json;
using System.Text;

namespace grupo4FarmaceuticaProyectoMov.Views;

public partial class vConsultas : ContentPage
{
    private readonly HttpClient _httpClient;

    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; }
    public string ApellidoUsuario { get; set; }

    public Usuario usuarioConsultaChat { get; set; }

    public vConsultas(Usuario usuario)
	{
		InitializeComponent();
        _httpClient = new HttpClient();
        IdUsuario = usuario.id;
        NombreUsuario = usuario.nombre;
        ApellidoUsuario = usuario.apellido;
        usuarioConsultaChat = usuario;

        lblBienvenido.Text = $"Bienvenido: {NombreUsuario} {ApellidoUsuario}";

        // Deshabilitar la flecha de retroceso en la p?gina de Login
        NavigationPage.SetHasBackButton(this, false);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        string pregunta = txtConsulta.Text;

        if (string.IsNullOrWhiteSpace(pregunta))
        {
            await DisplayAlert("Error", "Por favor ingresa una consulta.", "OK");
            return;
        }

        string apiUrl = $"http://{Config.LocalMachineIp}:8080/consulta/chat";

        // Crear el cuerpo de la solicitud como objeto an?nimo
        var requestBody = new { pregunta = pregunta };

        try
        {
            // Serializar el cuerpo a JSON usando System.Text.Json
            string jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Enviar la solicitud HTTP POST
            var response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                // Leer el contenido de la respuesta como texto plano
                string responseContent = await response.Content.ReadAsStringAsync();

                // Log para depuraci?n
                Console.WriteLine($"Respuesta Cruda: {responseContent}");

                // Crear un FormattedString para aplicar negrita
                FormattedString formattedString = new FormattedString();

                // Aqu?, asumimos que la respuesta est? en un formato predecible (cada l?nea corresponde a un campo).
                // Separar el texto en l?neas y luego agregar cada parte con formato.
                string[] lines = responseContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                // Recorremos las l?neas y agregamos cada l?nea como Span (con negrita en el t?tulo)
                foreach (var line in lines)
                {
                    if (line.StartsWith("Nombre:"))
                    {
                        formattedString.Spans.Add(new Span { Text = "Nombre: ", FontAttributes = FontAttributes.Bold });
                        formattedString.Spans.Add(new Span { Text = line.Replace("Nombre:", "").Trim() + "\n" });
                    }
                    else if (line.StartsWith("Presentaci?n:"))
                    {
                        formattedString.Spans.Add(new Span { Text = "Presentaci?n: ", FontAttributes = FontAttributes.Bold });
                        formattedString.Spans.Add(new Span { Text = line.Replace("Presentaci?n:", "").Trim() + "\n" });
                    }
                    else if (line.StartsWith("Dosis para ni?os:"))
                    {
                        formattedString.Spans.Add(new Span { Text = "Dosis para ni?os: ", FontAttributes = FontAttributes.Bold });
                        formattedString.Spans.Add(new Span { Text = line.Replace("Dosis para ni?os:", "").Trim() + "\n" });
                    }
                    else if (line.StartsWith("Dosis para adultos:"))
                    {
                        formattedString.Spans.Add(new Span { Text = "Dosis para adultos: ", FontAttributes = FontAttributes.Bold });
                        formattedString.Spans.Add(new Span { Text = line.Replace("Dosis para adultos:", "").Trim() + "\n" });
                    }
                    else if (line.StartsWith("Descripci?n:"))
                    {
                        formattedString.Spans.Add(new Span { Text = "Descripci?n: ", FontAttributes = FontAttributes.Bold });
                        formattedString.Spans.Add(new Span { Text = line.Replace("Descripci?n:", "").Trim() + "\n" });
                    }
                    else if (line.StartsWith("Indicaciones:"))
                    {
                        formattedString.Spans.Add(new Span { Text = "Indicaciones: ", FontAttributes = FontAttributes.Bold });
                        formattedString.Spans.Add(new Span { Text = line.Replace("Indicaciones:", "").Trim() + "\n" });
                    }
                    else if (line.StartsWith("Compuestos:"))
                    {
                        formattedString.Spans.Add(new Span { Text = "Compuestos: ", FontAttributes = FontAttributes.Bold });
                        formattedString.Spans.Add(new Span { Text = line.Replace("Compuestos:", "").Trim() + "\n" });
                    }
                    else if (line.StartsWith("Fecha de Expiraci?n ultimo lote:"))
                    {
                        formattedString.Spans.Add(new Span { Text = "Fecha de Expiraci?n ?ltimo lote: ", FontAttributes = FontAttributes.Bold });
                        formattedString.Spans.Add(new Span { Text = line.Replace("Fecha de Expiraci?n ultimo lote:", "").Trim() + "\n" });
                    }
                    else if (line.StartsWith("Precio:"))
                    {
                        formattedString.Spans.Add(new Span { Text = "Precio: ", FontAttributes = FontAttributes.Bold });
                        formattedString.Spans.Add(new Span { Text = line.Replace("Precio:", "").Trim() + "\n" });
                    }
                    else
                    {
                        formattedString.Spans.Add(new Span { Text = line + "\n" });
                    }
                }

                // Asignamos el texto formateado al Label
                lblRespuesta.FormattedText = formattedString;
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error de la API: {errorContent}");
                await DisplayAlert("Error", "No se pudo realizar la consulta. Int?ntalo m?s tarde.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurri? un error: {ex.Message}", "OK");
        }

    }

    private async void btnGuardar_Clicked(object sender, EventArgs e)
    {
        // Verifica que la consulta haya sido realizada y que haya una respuesta
        if (string.IsNullOrWhiteSpace(txtConsulta.Text) || lblRespuesta.FormattedText == null)
        {
            await DisplayAlert("Error", "No hay consulta realizada para guardar.", "OK");
            return;
        }

        string pregunta = txtConsulta.Text;  // Pregunta que el usuario envi?

        // Obtener el texto plano de los Spans en FormattedText
        StringBuilder respuestaBuilder = new StringBuilder();
        foreach (var span in lblRespuesta.FormattedText.Spans)
        {
            respuestaBuilder.Append(span.Text); // Concatenar el texto de cada Span
        }
        string respuesta = respuestaBuilder.ToString();  // El contenido de la respuesta como texto plano

        // Creamos el objeto RegistroConsultas
        var requestBody = new
        {
            contenido = respuesta,  // Guardamos la respuesta que obtuviste
            usuario = new
            {
                id = IdUsuario  // El ID del usuario que realiza la consulta
            }
        };

        string apiUrl = $"http://{Config.LocalMachineIp}:8080/registro/crear"; // URL para guardar la consulta

        try
        {
            // Serializar el cuerpo a JSON
            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Enviar la solicitud HTTP POST para guardar la consulta
            var response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("?xito", "La consulta ha sido guardada correctamente.", "OK");
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al guardar la consulta: {errorContent}");
                await DisplayAlert("Error", "No se pudo guardar la consulta. Int?ntalo m?s tarde.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurri? un error al guardar la consulta: {ex.Message}", "OK");
        }
    }

    private async void btnSalir_Clicked(object sender, EventArgs e)
    {
        var opciones = new Opciones(usuarioConsultaChat);
        await Navigation.PushAsync(opciones);
    }
}