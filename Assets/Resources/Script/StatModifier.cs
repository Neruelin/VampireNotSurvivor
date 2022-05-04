public class StatModifier {
    public float Mult;
    public float Flat;
    public string Tag;

    public StatModifier (string Tag, float Mult, float Flat) {
        this.Tag = Tag;
        this.Mult = Mult;
        this.Flat = Flat;
    }
}