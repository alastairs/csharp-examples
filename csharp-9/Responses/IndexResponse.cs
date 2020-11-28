using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace csharp_9.Responses
{
    public record IndexResponse
    {
#nullable disable // Disable compiler warnings about uninitialised properties being null
        [NotNull] public string CurrentUserUrl { get; init; }

        [NotNull] public string CurrentUserAuthorizationsHtmlUrl { get; init; }

        [NotNull] public string AuthorizationsUrl { get; init; }

        [NotNull] public string CodeSearchUrl { get; init; }

        [NotNull] public string CommitSearchUrl { get; init; }

        [NotNull] public string EmailsUrl { get; init; }

        [NotNull] public string EmojisUrl { get; init; }

        [NotNull] public string EventsUrl { get; init; }

        [NotNull] public string FeedsUrl { get; init; }

        [NotNull] public string FollowersUrl { get; init; }

        [NotNull] public string FollowingUrl { get; init; }

        [NotNull] public string GistsUrl { get; init; }

        [NotNull] public string HubUrl { get; init; }

        [NotNull] public string IssueSearchUrl { get; init; }

        [NotNull] public string IssuesUrl { get; init; }

        [NotNull] public string KeysUrl { get; init; }

        [NotNull] public string LabelSearchUrl { get; init; }

        [NotNull] public string NotificationsUrl { get; init; }

        [NotNull] public string OrganizationUrl { get; init; }

        [NotNull] public string OrganizationRepositoriesUrl { get; init; }

        [NotNull] public string OrganizationTeamsUrl { get; init; }

        [NotNull] public string PublicGistsUrl { get; init; }

        [NotNull] public string RatelimitUrl { get; init; }

        [NotNull] public string RepositoryUrl { get; init; }

        [NotNull] public string RepositorySearchUrl { get; init; }

        [NotNull] public string CurrentUserRepositoriesUrl { get; init; }

        [NotNull] public string StarredUrl { get; init; }

        [NotNull] public string StarredGistsUrl { get; init; }

        [NotNull] public string UserUrl { get; init; }

        [NotNull] public string UserOrganizationsUrl { get; init; }

        [NotNull] public string UserRepositoriesUrl { get; init; }

        [NotNull] public string UserSearchUrl { get; init; }
#nullable restore // Restore the nullable setting
    }
}