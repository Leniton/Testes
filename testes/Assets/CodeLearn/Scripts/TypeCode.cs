using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;

public class TypeCode : MonoBehaviour
{

	public static TypeCode Ointerface;

	[SerializeField]
	List<GameObject> sugestoes;
	[SerializeField]
	GameObject objsugest;
	[Space]
	[SerializeField]
	GameObject PCodigo;
	List<GameObject> codigos;
	[Space]
	[SerializeField]
	ObjetoCodigo objeto;

	List<string> sugest;
	bool NotFound;
	bool editavel;

	void Start()
	{
		Ointerface = this;
		sugest = new List<string>();
		codigos = new List<GameObject>();
	}

	public void IniciarInterface(ObjetoCodigo obj)
	{
		print("iniciou");
		objeto = obj;
		editavel = objeto.Editable;
		GetComponent<InputField>().interactable = editavel;

		if (codigos.Count > 0)
		{
			for (int i = 0; i < codigos.Count; i++)
			{
				Destroy(codigos[i]);
			}
			codigos.Clear();
		}

		if (obj.codigos.Count > 0)
		{
			for (int i = 0; i < obj.codigos.Count; i++)
			{
				AdicionarCodigo(obj.codigos[i]);
			}
		}
	}


	#region digitar/sugestão
	public void checarSugestao(string texto)
	{
		print(texto);
		List<string> lista = ListCodes.Tlist.LCodigos.ToList();
		LimparSugestao();
		if (texto != "")
		{
			for (int i = 0; i < texto.Length; i++)
			{
				for (int l = 0; l < lista.Count; l++)
				{
					if (i < lista[l].Length)
					{
						/*print("caractere: " + texto[i] + ", n:" + i + " / " + (texto[i] == lista[l][i] || char.ToUpper(texto[i]) == lista[l][i]));
                        print(!sugest.Contains(lista[l]));
                        print(texto.Length <= lista[l].Length);*/
						if ((texto[i] == lista[l][i] || char.ToUpper(texto[i]) == lista[l][i]) &&
						    texto.Length <= lista[l].Length)
						{
							//print("tem similar");
							if (!sugest.Contains(lista[l]))
							{
								sugest.Add(lista[l]);
							}
						}
						else
						{
							sugest.Remove(lista[l]);
							lista.RemoveAt(l);
							l--;
						}
					}
					if (l >= lista.Count - 1 && sugest.Count == 0)
					{
						//print("não tem nenhum");
						sugest.Add("Nenhum comando similar encontrado");
						NotFound = true;
						break;
					}
				}
				if (NotFound)
				{
					break;
				}
			}

			for (int i = 0; i < sugest.Count; i++)
			{
				Vector3 pos = new Vector3(objsugest.transform.position.x, objsugest.transform.position.y, objsugest.transform.position.z);
				pos = new Vector3(pos.x, pos.y + (objsugest.GetComponent<RectTransform>().rect.height * (i)), pos.z);
				sugestoes.Add(Instantiate(objsugest, transform));
				sugestoes[i].transform.position = pos;
				sugestoes[i].GetComponentInChildren<Text>().text = sugest[i];
				sugestoes[i].SetActive(true);
				// + (posit.offsetMax.y * (1 + i))
			}
		}
	}

	public void LimparSugestao()
	{
		for (int i = 0; i < sugestoes.Count; i++)
		{
			GameObject aDestruir = sugestoes[i];
			sugestoes.Remove(aDestruir);
			Destroy(aDestruir);
		}
		sugest.Clear();
		NotFound = false;
	}

	public void AplicarSugetsao(Text sugestao)
	{
		print("aplicar");
		GetComponent<InputField>().text = sugestao.text;
		LimparSugestao();
	}
#endregion

	//parte do "código" já adicionado

	public void AdicionarCodigo(string txt)
	{
		Vector3 pos = new Vector3(PCodigo.transform.position.x, PCodigo.transform.position.y, 0);
		pos = new Vector3(pos.x, pos.y - (PCodigo.GetComponent<RectTransform>().rect.height + 2)*codigos.Count, pos.z);
		codigos.Add(Instantiate(PCodigo, PCodigo.transform.parent));
		codigos[codigos.Count - 1].transform.position = pos;
		codigos[codigos.Count - 1].GetComponentInChildren<Text>().text = txt;
		codigos[codigos.Count - 1].SetActive(true);
		if (editavel)
		{
			codigos[codigos.Count - 1].transform.GetChild(2).gameObject.SetActive(true);
		}
	}

	public void AdicionarCodigo(Text txt)
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			Vector3 pos = new Vector3(PCodigo.transform.position.x, PCodigo.transform.position.y, 0);
			pos = new Vector3(pos.x, pos.y - (PCodigo.GetComponent<RectTransform>().rect.height + 2) * codigos.Count, pos.z);
			codigos.Add(Instantiate(PCodigo, PCodigo.transform.parent));
			codigos[codigos.Count - 1].transform.position = pos;
			codigos[codigos.Count - 1].GetComponentInChildren<Text>().text = txt.text;
			codigos[codigos.Count - 1].SetActive(true);
			if (editavel)
			{
				codigos[codigos.Count - 1].transform.GetChild(2).gameObject.SetActive(true);
			}
		}
	}

	public void RemoverCodigo(GameObject codigo)
	{
		codigos.Remove(codigo);
		Destroy(codigo);
		if (codigos.Count > 0)
		{
			for (int i = 0; i < codigos.Count; i++)
			{
				Vector3 pos = new Vector3(PCodigo.transform.position.x, PCodigo.transform.position.y, 0);
				pos = new Vector3(pos.x, pos.y - (PCodigo.GetComponent<RectTransform>().rect.height + 2) * i, pos.z);
				codigos[i].transform.position = pos;
			}
		}
	}

	public void fecharInterface()
	{
		if (codigos.Count > 0)
		{
			List<string> listaCodigos = new List<string>();
			for (int i = 0; i < codigos.Count; i++)
			{
				listaCodigos.Add(codigos[i].GetComponentInChildren<Text>().text);
			}
		}
	}

}
