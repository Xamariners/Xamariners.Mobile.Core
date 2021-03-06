using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamariners.Mobile.Core.Interfaces
{
   public interface IPopupInputService
    {
        string OkButtonText { get; set; }
        string MessageText { get; set; }
        string TitleText { get; set; }
        string PlaceHolderText { get; set; }
        string ValidationLabelText { get; set; }
        string CancelButtonText { get; set; }
        bool IsShowing { get; set; }

        /// <summary>
        /// Display a Text Input Alert Dialogbox
        /// </summary>
        /// <param name="titleText"></param>
        /// <param name="messageText"></param>
        /// <param name="placeHolderText"></param>
        /// <param name="okButtonText"></param>
        /// <param name="cancelButtonText"></param>
        /// <param name="validationLabelText"></param>
        /// <returns></returns>
        Task<string> ShowInputTextOkCancelAlertPopup(
            string titleText,
            string messageText,
            string placeHolderText,
            string okButtonText,
            string cancelButtonText,
            string validationLabelText);

        /// <summary>
        /// Show Message Alert popup with ok/Cancel button
        /// </summary>
        /// <param name="titleText"></param>
        /// <param name="messageText"></param>
        /// <param name="okButtonText"></param>
        /// <param name="cancelButtonText"></param>
        /// <returns></returns>
        Task<bool> ShowMessageOkCancelAlertPopup(
            string titleText,
            string messageText,
            string okButtonText,
            string cancelButtonText);

        /// <summary>
        /// Show Message Alert popup with Ok button
        /// </summary>
        /// <param name="titleText"></param>
        /// <param name="messageText"></param>
        /// <param name="okButtonText"></param>
        /// <returns></returns>
        Task<string> ShowMessageOkAlertPopup(
            string titleText,
            string messageText,
            string okButtonText);

        /// <summary>
        /// Display a Input Selection Alert Dialogbox
        /// </summary>
        /// <param name="titleText"></param>
        /// <param name="selectionList"></param>
        /// <param name="cancelButtonText"></param>
        /// <returns></returns>
        Task<string> ShowInputSelectionCancelAlertPopup(
            string titleText,
            List<string> selectionList,
            string cancelButtonText);

        /// <summary>
        /// Display any given Xamarin.Forms.View in a transparent popup
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewObject"></param>
        /// <returns></returns>
        Task<T> ShowCustomViewAlertPopup<T>(object viewObject);

        /// <summary>
        /// Close the last popup in the popup stack.
        /// </summary>
        /// <returns>An await-able <see cref="Task"/>.</returns>
        Task CloseLastPopup();

        /// <summary>
        /// Show markdown popup view
        /// </summary>
        /// <param name="markDownText"></param>
        /// <returns></returns>
        Task ShowInfoMarkdownViewPopup(string markDownText, ImageSource imageSource);
    }
}
