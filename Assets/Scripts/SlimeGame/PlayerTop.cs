using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Qrio
{
	public class PlayerTop : MonoBehaviour
	{
		List<TextMeshProUGUI> scores = new List<TextMeshProUGUI>();

		void Start()
		{

			foreach (var text in GetComponentsInChildren<TextMeshProUGUI>())
            {
				text.text = "";
				scores.Add(text);
            }
		}

		public void SetTexts(List<SlimeController> players)
        {
			SlimeController[] top = players
				.Where(p => !p.isDead)
				.OrderByDescending(p => p.maxHeight)
				.Take(3)
				.ToArray();

			SlimeController myCharacter;
			float myScore = 0;
			if (players.FirstOrDefault(p => p != null))
            {
				myCharacter = players.FirstOrDefault(p => p.photonView.IsMine);
				myScore = myCharacter.maxHeight;
            }

			scores[0].text = "You: " + (int)myScore + "\n";

			for (int i = 0; i < top.Length; i++)
				scores[i + 1].text = (i + 1) + ". " + top[i].photonView.Owner.NickName + ": " + (int)top[i].maxHeight;

		}
	}
}