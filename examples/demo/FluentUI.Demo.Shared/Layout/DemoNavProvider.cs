using Microsoft.AspNetCore.Components.Routing;
using Microsoft.FluentUI.AspNetCore.Components;
using Icons = Microsoft.FluentUI.AspNetCore.Components.Icons;

namespace FluentUI.Demo.Shared.Layout;

public class DemoNavProvider
{

    public IReadOnlyList<NavItem> NavMenuItems { get; init; }

    public IReadOnlyList<NavItem> FlattenedMenuItems { get; init; }

    public DemoNavProvider()
    {
        NavMenuItems =
        [
            new NavLink(
                href: "",
                match: NavLinkMatch.All,
                icon: new Icons.Regular.Size20.Home(),
                title: "Home"
            ),
            new NavGroup(
                icon: new Icons.Regular.Size20.ContentViewGallery(),
                title: "Layout",
                expanded: false,
                gap: "10px",
                children:
                [
                    new NavLink(
                        href: "devicedetector",
                        icon: new Icons.Regular.Size20.PhoneDesktop(),
                        title: "Device detector"
                    ),
                    new NavLink(
                        href: "tilegrid",
                        icon: new Icons.Regular.Size20.Apps(),
                        title: "Tile Grid"
                    ),
                    new NavLink(
                        href: "mediaquery",
                        icon: new Icons.Regular.Size20.PhoneDesktop(),
                        title: "Media queries"
                    ),
                    new NavLink(
                        href: "resizers",
                        icon: new Icons.Regular.Size20.Resize(),
                        title: "Resizers"
                    ),
                ]),
            new NavGroup(
                icon: new Icons.Regular.Size20.PuzzleCubePiece(),
                title: "Components",
                expanded: false,
                gap: "10px",
                children:
               [
                    new NavGroup(
                        icon: new Icons.Regular.Size20.FilmstripImage(),
                        title: "Animations",
                        gap: "10px",
                        expanded: false,
                        children: [
                            new NavLink(
                            href: "animations-overview",
                          icon: new Icons.Regular.Size20.FilmstripImage(),
                          title: "Overview"
                          ),
                          new NavLink(
                            href: "animation-default",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Default"
                          ),
                          new NavLink(
                            href: "animation-bindstack",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Bindstack"
                          ),
                          new NavLink(
                            href: "animation-cascade",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Cascade"
                          ),
                          new NavLink(
                            href: "animation-chaos",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Chaos"
                          ),
                          new NavLink(
                            href: "animation-fan",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Fan"
                          ),
                          new NavLink(
                            href: "animation-float",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Float"
                          ),
                          new NavLink(
                            href: "animation-flower",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Flower"
                          ),
                          new NavLink(
                            href: "animation-galaxy",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Galaxy"
                          ),
                          new NavLink(
                            href: "animation-goldenspiral",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Golden Spiral"
                          ),
                          new NavLink(
                            href: "animation-grid",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Grid"
                          ),
                          new NavLink(
                            href: "animation-heart",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Heart"
                          ),
                          new NavLink(
                            href: "animation-helix",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Helix"
                          ),
                          new NavLink(
                            href: "animation-magnet",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Magnet"
                          ),
                          new NavLink(
                            href: "animation-morphing",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Morphing"
                          ),
                          new NavLink(
                            href: "animation-orbit",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Orbit"
                          ),
                          new NavLink(
                            href: "animation-pin",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Pin"
                          ),
                          new NavLink(
                            href: "animation-pulse",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Pulse"
                          ),
                          new NavLink(
                            href: "animation-snake",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Snake"
                          ),
                          new NavLink(
                            href: "animation-spiral-galaxy",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Spiral Galaxy"
                          ),
                          new NavLink(
                            href: "animation-spiral",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Spiral"
                          ),
                          new NavLink(
                            href: "animation-stacked-rotating",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Stacked Rotating"
                          ),
                          new NavLink(
                            href: "animation-stack",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Stack"
                          ),
                          new NavLink(
                            href: "animation-sunburst",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Sunburst"
                          ),
                          new NavLink(
                            href: "animation-vortex",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Vortex"
                          ),
                          new NavLink(
                            href: "animation-wave",
                            icon: new Icons.Regular.Size20.FilmstripImage(),
                            title: "Wave"
                          ),
                        ]
                    ),
                    new NavLink(
                        href: "cookies",
                        icon: new Icons.Regular.Size20.Cookies(),
                        title: "Cookies"
                    ),
                    new NavLink(
                        href: "filemanager",
                        icon: new Icons.Regular.Size20.FolderOpen(),
                        title: "File Manager"
                    ),
                    new NavLink(
                        href: "floatingbutton",
                        icon: new Icons.Regular.Size20.Button(),
                        title: "Floating button"
                    ),
                    new NavLink(
                        href: "imagegroup",
                        icon: new Icons.Regular.Size20.ImageMultiple(),
                        title: "Image group"
                    ),
                    new NavLink(
                        href: "pathbar",
                        icon: new Icons.Regular.Size20.Navigation(),
                        title: "Path bar"
                    ),
                    new NavLink(
                        href: "sleekdial",
                        icon: new Icons.Regular.Size20.RadioButton(),
                        title: "Sleek dial"
                    ),
                    new NavLink(
                        href: "slideshow",
                        icon: new Icons.Regular.Size20.SlideTransition(),
                        title: "Slideshow"
                    ),
                ]),
            new NavGroup(
                icon: new Icons.Regular.Size20.Beaker(),
                title: "Incubation lab",
                expanded: false,
                gap: "10px",
                children:
                [
                    new NavLink(
                        href: "Lab/Overview",
                        icon: new Icons.Regular.Size20.Beaker(),
                        title: "Overview"
                    ),

                    new NavLink(
                        href: "issue-tester",
                        icon: new Icons.Regular.Size20.WrenchScrewdriver(),
                        title: "Issue Tester"
                    ),
                ]
            )
        ];

        FlattenedMenuItems = GetFlattenedMenuItems(NavMenuItems)
            .ToList()
            .AsReadOnly();
    }

    private static IEnumerable<NavItem> GetFlattenedMenuItems(IEnumerable<NavItem> items)
    {
        foreach (var item in items)
        {
            yield return item;

            if (item is not NavGroup group || !group.Children.Any())
            {
                continue;
            }

            foreach (var flattenedMenuItem in GetFlattenedMenuItems(group.Children))
            {
                yield return flattenedMenuItem;
            }
        }
    }
}
