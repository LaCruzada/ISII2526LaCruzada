using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
    }

    public ApplicationUser(string userName) : base(userName)
    {
    }

    public ApplicationUser(string nombre, string apellido1, string apellido2)
    {

        Nombre = nombre;
        Apellido1 = apellido1;
        Apellido2 = apellido2;
    }

    [Required]
    [StringLength(50)]
    public string Nombre { get; set; }

    [Required]
    [StringLength(50)]
    public string Apellido1 { get; set; }

    [StringLength(50)]
    public string Apellido2 { get; set; }

 
    public List<CompraBono> Compras { get; set; }

    public string? DireccionEnvio { get; set; } 
}