// This is a demo file for TypeScript code used in the FluentUI.Blazor.Community project.
export namespace FluentUI.Blazor.Community.FloatingButton {
  export function HasValidTarget(targetId: string) {
    if (!targetId) {
      return false;
    }

    var element = document.getElementById(targetId);

    return element != null && element !== undefined;
  }
}
