using Ulak.Domain.Common;

namespace Ulak.Domain.Entities;

public class Kullanici : BaseEntity
{
    public string Ad { get; set; } = null!;

    public string Soyad { get; set; } = null!;

    public string Email { get; set; } = null!;
}