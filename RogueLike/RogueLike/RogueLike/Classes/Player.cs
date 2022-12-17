using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Player : Entity
{
    public Player(Viewport viewport) :
        base(viewport)
    {
        position = new Vector2(viewport.Width / 2, viewport.Height / 2);
        weapon = new Weapon(); // nur für Testzwecke, zu Beginn des Spiels hat der Spieler keine Waffe (= Fäuste also null)
    }
}
