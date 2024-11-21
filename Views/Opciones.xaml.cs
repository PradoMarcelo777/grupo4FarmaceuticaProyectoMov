using grupo4FarmaceuticaProyectoMov.Models;
using Microsoft.Win32;
namespace grupo4FarmaceuticaProyectoMov.Views;

public partial class Opciones : ContentPage
{

    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; }
    public string ApellidoUsuario { get; set; }

    public Opciones(Usuario usuario)
    {
        InitializeComponent();
        
        IdUsuario = usuario.id;
        NombreUsuario = usuario.nombre;
        ApellidoUsuario = usuario.apellido;

        
        lblBienvenido.Text = $"Bienvenido: {NombreUsuario} {ApellidoUsuario}";

        
        NavigationPage.SetHasBackButton(this, false);

    }

    private async void btnMedicamentos_Clicked(object sender, EventArgs e)
    {
        
        var usuario = new Usuario

        {
            id = IdUsuario,
            nombre = NombreUsuario,
            apellido = ApellidoUsuario
        };

        await Navigation.PushAsync(new PrincipalMedicamento(usuario));
    }

    private async void btnConsulta_Clicked(object sender, EventArgs e)
    {
        var usuario = new Usuario

        {
            id = IdUsuario,
            nombre = NombreUsuario,
            apellido = ApellidoUsuario
        };

        await Navigation.PushAsync(new vConsultas(usuario));
    }

    private async void btnHistorial_Clicked(object sender, EventArgs e)
    {
        var usuario = new Usuario

        {
            id = IdUsuario,
            nombre = NombreUsuario,
            apellido = ApellidoUsuario
        };

        await Navigation.PushAsync(new vRegistro(usuario));
    }

    private async void btnSalir_Clicked(object sender, EventArgs e)
    {


        
        Preferences.Remove("username");
        Preferences.Remove("password");

        
        await Navigation.PushAsync(new Login());
    }
}