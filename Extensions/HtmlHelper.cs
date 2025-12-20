using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using CampusActivityHub.Models;

namespace CampusActivityHub.Extensions
{
    public static class HtmlHelpers
    {
        public static IHtmlContent EventStatus(this IHtmlHelper html, Event eventItem)
        {
            string badgeClass, statusText;

            if (eventItem.Date < DateTime.Now)
            {
                badgeClass = "bg-secondary";
                statusText = "Завершено";
            }
            else if (eventItem.Registrations.Count >= eventItem.MaxParticipants)
            {
                badgeClass = "bg-danger";
                statusText = "Sold Out";
            }
            else
            {
                badgeClass = "bg-success";
                statusText = "Активна";
            }

            var htmlContent = $"<span class='badge {badgeClass} rounded-pill'>{statusText}</span>";
            return new HtmlString(htmlContent);
        }
    }
}