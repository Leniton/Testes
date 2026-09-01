using System;

namespace Util
{
    public class Checklist
    {
        public int requiredSteps { get; private set; }
        public int currentSteps { get; private set; }

        public float percentDone => (float)currentSteps / requiredSteps;
        public bool isDone => currentSteps >= requiredSteps;
        public event Action<float> onProgress;
        public event Action onCompleted;

        public Checklist(int stepsNeeded)
        {
            currentSteps = 0;
            requiredSteps = stepsNeeded;
        }

        public void AddStep(int amount = 1) => requiredSteps += amount;

        public void FinishStep() => FinishSteps();

        public void FinishSteps(int stepAmount = 1)
        {
            currentSteps += stepAmount;
            currentSteps = Math.Min(currentSteps, requiredSteps);
            onProgress?.Invoke(percentDone);
            if (isDone) onCompleted?.Invoke();
        }

        public void ResetChecklist() => currentSteps = 0;
    }
}