using Lessons.Gameplay;

public interface IGetLifeComponent
{
    HeroModel_Core.Life GetLifeComponent();
}

public class LifeComponent : IGetLifeComponent
{
    private readonly HeroModel_Core.Life _lifeComp;
    
    public LifeComponent(HeroModel_Core.Life lifeComp)
    {
        _lifeComp = lifeComp;
    }
    
    public HeroModel_Core.Life GetLifeComponent()
    {
        return _lifeComp;
    }
}
