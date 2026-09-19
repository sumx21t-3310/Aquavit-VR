# Third-Party Notices

This file records third-party material that Aquavit VR ports, bundles, or depends on. The rules for what may be referenced and ported are in [docs/ReferencePolicy.md](docs/ReferencePolicy.md).

## Ported or bundled code

None. Aquavit VR contains no code ported from, or bundled with, another project.

When you port code, add a section for the source project below, in the same change as the port. Each section states:

- the project name and URL,
- the license (SPDX identifier) and the full license text with its copyright notice,
- which files in this repository contain the ported code, and the source commit or version.

Only projects listed under "開いてよいもの" in [docs/ReferencePolicy.md](docs/ReferencePolicy.md) may be ported.

## Design influence

Aquavit VR renders through the LayerTree of [FloatSoda](https://github.com/sumx21t-3310/FloatSoda) (MIT, same author) and follows FloatSoda's repository conventions. Aquavit VR references FloatSoda as a NuGet package and contains no FloatSoda source code.

The DOM, CSS, and event models follow the WHATWG and W3C specifications. Aquavit VR is developed without reference to the source code of WebF, Kraken, Blink, WebKit, Servo, or Gecko.

## NuGet dependencies

License expressions were read from each package's `.nuspec` on nuget.org on 2026-09-20. Packages are restored from nuget.org at build time and are not redistributed in this repository.

### Runtime dependencies of the `Aquavit` package

| Package | Version | License |
|---|---|---|
| FloatSoda.Rendering | 0.3.1 | MIT |
| SkiaSharp (transitive, including its native assets packages) | 3.119.2 | MIT |

### Build-time and test-only dependencies

These packages are referenced with `PrivateAssets="all"` or from test projects only. They are not dependencies of the `Aquavit` package.

| Package | Version | License |
|---|---|---|
| Roslynator.Analyzers | 4.13.1 | Apache-2.0 |
| StyleCop.Analyzers | 1.2.0-beta.556 | MIT |
| xunit | 2.9.3 | Apache-2.0 |
| xunit.runner.visualstudio | 3.1.4 | Apache-2.0 |
| Microsoft.NET.Test.Sdk | 17.14.1 | MIT |
| coverlet.collector | 6.0.4 | MIT |

When you add or update a dependency, confirm that its license is compatible with MIT distribution (MIT, BSD, Apache-2.0) and update the tables above in the same change.
