public class EffectInstance
{
    private EffectSO effectSO;
    private float durationRemaining;
    private float totalDuration;
    private float period;
    private bool executeOnApplication;
    private float timeUntilNextTick;
    private AbilityCharacter caster;
    private AbilityCharacter target;
    public float statChange;

    public EffectSO EffectSO => effectSO;   
    public float DurationRemaining => durationRemaining;
    public AbilityCharacter Target => target;
    public float Period => period;
    public bool ExecuteOnApplication => executeOnApplication;
    public float TimeUntilNextTick => timeUntilNextTick;
    public AbilityCharacter Caster => caster;
    public float TotalDuration => totalDuration;



    public static EffectInstance CreateNew(EffectSO effect, AbilityCharacter caster, AbilityCharacter target = null)
    {
        return new EffectInstance(effect, caster, target);
    }

    public EffectInstance(EffectInstance instance)
    {         
        this.effectSO = instance.effectSO;
        this.durationRemaining = instance.durationRemaining;
        this.totalDuration = instance.totalDuration;
        this.period = instance.period;
        this.executeOnApplication = instance.executeOnApplication;
        this.timeUntilNextTick = instance.timeUntilNextTick;
        this.caster = instance.caster;
        this.target = instance.target;
    }

    public EffectInstance(EffectSO effect, AbilityCharacter caster, AbilityCharacter target = null)
    {
        this.effectSO = effect;
        this.caster = caster;
        this.target = target;
        for (int i = 0; i < effect.EffectModifiers.Length; i++)
        {
            effect.EffectModifiers[i].ModifierMagnitude.Init(this);
        }
        if (this.effectSO.DurationMagnitude)
        {
            this.durationRemaining = this.effectSO.DurationMagnitude.CalculateMagnitude(this) * this.effectSO.DurationMultiplier;
            this.totalDuration = this.DurationRemaining;
        }

        this.timeUntilNextTick = this.effectSO.Period;
        // By setting the time to 0, it gets executed at first opportunity
        if (this.effectSO.ExecuteOnApplication)
        {
            this.timeUntilNextTick = 0;
        }
    }

    public EffectInstance TickPeriodic(float deltaTime, out bool executePeriodicTick)
    {
        this.timeUntilNextTick -= deltaTime;
        executePeriodicTick = false;
        if (this.timeUntilNextTick <= 0)
        {
            this.timeUntilNextTick = this.EffectSO.Period;

            // Check to make sure period is valid, otherwise we'd just end up executing every frame
            if (this.EffectSO.Period > 0)
            {
                executePeriodicTick = true;
            }
        }
        return this;
    }
    public EffectInstance SetTarget(AbilityCharacter target)
    {
        this.target = target;
        return this;
    }

    public void SetTotalDuration(float totalDuration)
    {
        this.totalDuration = totalDuration;
    }

    public EffectInstance SetDuration(float duration)
    {
        this.durationRemaining = duration;
        return this;
    }

    public EffectInstance UpdateRemainingDuration(float deltaTime)
    {
        this.durationRemaining -= deltaTime;
        return this;
    }

}

public enum EStatChange
{
    Max,
    Current,
    Both
}

