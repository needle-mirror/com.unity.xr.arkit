---
uid: arkit-light-estimation
---
# Light estimation

ARKit light estimation can only be `enabled` or `disabled`. The availability of either  `Ambient Intensity` or `Environmental HDR` data is governed by the active tracking mode. See the following table for more details.

| Tracking configuration | Ambient intensity (lumens) | Color temperature | Main light direction | Main light intensity (lumens) | Ambient spherical harmonics |
|------------------------|----------------------------|-------------------|----------------------|-------------------------------|-----------------------------|
| World Tracking         | Yes                        | Yes               | No                   | No                            | No                          |
| Face Tracking          | Yes                        | Yes               | Yes                  | Yes                           | Yes                         |
