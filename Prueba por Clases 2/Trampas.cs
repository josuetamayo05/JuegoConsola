

public class Trampa
{
    public int[] Position { get; set; } 
    public bool Visible { get; set; }
    public bool HaSidoVisualizada { get; set; }

    public Trampa(int fila, int columna)
    {
        Position = new int[] { fila, columna };
        Visible = true;
        HaSidoVisualizada = false;
    }

    public void ToggleVisibility()
    {
        Visible = !Visible;
    }
}