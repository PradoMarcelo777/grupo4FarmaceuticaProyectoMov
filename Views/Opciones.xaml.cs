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
    protected override bool OnBackButtonPressed()
    {
        // No hacer nada para deshabilitar el retroceso
        return true; // True indica que el evento est? controlado y se ignora la acci?n predeterminada
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

        bool confirm = await DisplayAlert(
         "Cerrar Sesión",
         "Estás seguro de que deseas cerrar tu sesión?",
         "Sí",
         "No"
     );

        if (!confirm)
        {
            // Si el usuario selecciona "No", salir del m?todo
            return;
        }

        Preferences.Remove("username");
        Preferences.Remove("password");

        await DisplayAlert("Sesión Cerrada", "Tu sesión se ha cerrado correctamente.", "OK");


        await Navigation.PushAsync(new Login());
    }
}