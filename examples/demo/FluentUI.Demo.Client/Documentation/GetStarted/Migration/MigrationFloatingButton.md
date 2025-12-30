---
title: Migration FloatingButton
route: /Migration/FloatingButton
hidden: true
---

### New properties

- `IsFixed` can be used to position the control fixed to the DOM.
- The component now features all new properties from `Microsoft.FluentUI.AspNetCore.Components` v5

### Removed properties 💥

- `RelativeContainerId` (the component now uses the parent container by default. Except when you set `IsFixed`
- `OnMouseEnter` use `@onmouseenter` instead
- `OnKeyDown` use `@onkeydown` instead

