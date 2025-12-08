export namespace FluentUI.Blazor.Community.FloatingButton {
  export function HasValidTarget(targetId: string) {
    if (!targetId) {
      return false;
    }

    var element = document.getElementById(targetId);

    return element != null && element !== undefined;
  }
}
