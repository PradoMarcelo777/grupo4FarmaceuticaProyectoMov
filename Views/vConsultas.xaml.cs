using grupo4FarmaceuticaProyectoMov.Models;

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

    private void Button_Clicked(object sender, EventArgs e)
    {

    }

    private void btnGuardar_Clicked(object sender, EventArgs e)
    {

    }

    private void btnSalir_Clicked(object sender, EventArgs e)
    {

    }
}