using System.ComponentModel.DataAnnotations.Schema;

namespace Database;

public class Vector
{
    [Column(Order = 0)]
    [ForeignKey("Image")]
    public int ImageId { get; set; }

    [Column(Order = 1)] public int VectorPosition { get; set; }

    public float VectorValue { get; set; }

    public virtual Image Image { get; set; }
}