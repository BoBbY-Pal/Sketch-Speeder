using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Sketch_Speeder.UI
{
    public class PopUp : MonoBehaviour

    {
        public Text titleTxt;
        public Text description;
        public Button okBtn;

        private RectTransform thisRectTransform;
        private void Awake()
        {
            thisRectTransform = transform.GetComponent<RectTransform>();
            okBtn.onClick.AddListener(DisablePopUp);
        }

        private void OnEnable()
        {
            thisRectTransform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InElastic);
        }

        private void OnDisable()
        {
            thisRectTransform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutBounce);
        }

        void DisablePopUp()
        {
            gameObject.SetActive(false);
        }
        
       public IEnumerator DisablePopUp(float timer)
        {
            yield return new WaitForSeconds(timer);
            gameObject.SetActive(false);
        }
    }
}