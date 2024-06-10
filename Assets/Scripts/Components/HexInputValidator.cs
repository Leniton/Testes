using UnityEngine;
using TMPro;
/// <summary>
/// Create your validator class and inherit TMPro.TMP_InputValidator 
/// Note that this is a ScriptableObject, so you'll have to create an instance of it via the Assets -> Create -> Input Field Validator 
/// </summary>
[CreateAssetMenu(fileName = "Input Field Validator", menuName = "TextMesh Pro/Input Field Validator")]
public class CustomValidator : TMP_InputValidator
{
    [SerializeField] private bool CaseSensitive;
    [SerializeField] private char[] AllowedChars;

    /// <summary>
    /// Override Validate method to implement your own validation
    /// </summary>
    /// <param name="text">This is a reference pointer to the actual text in the input field; changes made to this text argument will also result in changes made to text shown in the input field</param>
    /// <param name="pos">This is a reference pointer to the input field's text insertion index position (your blinking caret cursor); changing this value will also change the index of the input field's insertion position</param>
    /// <param name="ch">This is the character being typed into the input field</param>
    /// <returns>Return the character you'd allow into </returns>
    public override char Validate(ref string text, ref int pos, char ch)
    {
        //Debug.Log($"Text = {text}; pos = {pos}; chr = {ch}");
        char currentChar = CaseSensitive ? ch : char.ToLower(ch);
        for (int i = 0; i < AllowedChars.Length; i++)
        {
            char character = CaseSensitive ? AllowedChars[i] : char.ToLower(AllowedChars[i]);
            if (currentChar == character)
            {
#if UNITY_EDITOR
                text = text.Insert(pos, ch.ToString());
#endif
                
                pos++;
                return ch;
            }
        }
        return '\0';
    }
}