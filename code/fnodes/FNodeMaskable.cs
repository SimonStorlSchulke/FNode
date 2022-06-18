using Godot;
using System.Linq;

public abstract class FNodeMaskable : FNode {

    protected FInput passthrough;
    protected bool MaskHidden;

    public FNodeMaskable() {
        
    }

    
    public override void _Ready() {
        GD.Print(inputs.Count);
        inputs.Add("Mask", new FInputBool(this, 0, initialValue: true));
        passthrough = inputs.ElementAt(0).Value;
        base._Ready();
    }

    //public void ToggleMask() {

    ///<summary> Passthrough Nodes must call base.OnBeforeEvaluation()</summary>
    public override void OnNextIteration() {
        if ((bool)inputs["Mask"].Get()) {
            outputs.ElementAt(0).Value.Get = delegate() {
                return passthrough.Get();
            };
        }
    }

}

