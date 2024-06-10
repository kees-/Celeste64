# Mapping notes and information

## Profiling

I'm using the collision map of "Kiln of the First Flame" as a sample map. It contains 34.5k faces.

### Kiln of the First Flame sample map

| Method  | Time  | Notes                                                     |
| ------- | ----- | --------------------------------------------------------- |
| Brush   | 1s    | Inappropriate for large maps: single brush per mesh       |
| Faces   | 818s  | Accurate and semi effective, explodes on concave geometry |
| Walls   | 3784s | Sloooowwww and accurate; nice handling of shared edges    |
| Terrain | 2568s | ❌ Nope this ain't it                                     |
| Blob    | 848s  | ❌ Hangs                                                  |
| Shell   | 2505s | ❌ Hangs                                                  |
| Patches | 3s    | ❌ Different format crashes startup asset load            |

- Face method does not handle intersecting edges well and creates lots of shader artifacts.
- Wall method seems to have sommeee edge overlap occasionally? Or overhanging textures? Can't tell

### Script improvements

Exporting Kiln with Faces method for consistency.

- 818s / XL: unchanged
- 852s / XL: Condensed sequential `fw` calls
- 767s / XL: Simplify control flow and comment out `print` call

Exporting the full Ash Lake with Faces

- 6255 / XL
