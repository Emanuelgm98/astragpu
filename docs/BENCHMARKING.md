# Benchmark methodology

AstraGPU currently provides only summary calculations; it does not launch games or capture frame times. Empty datasets return no numeric result. The analyzer uses nearest-rank percentiles; a 1% low estimate is emitted only for at least 100 frame-time samples and is calculated as `1000 / p99 frame time (ms)`.

For meaningful future comparisons, record game/build, scene or replay, resolution, graphics settings, driver, power mode, capture provider/version, warm-up, run duration, and ambient conditions. Run multiple passes with the same setup and report individual runs plus median. Compare p95/p99 frametime as well as averages. Do not interpret results as universal FPS gains. Current tests use fixtures solely to verify arithmetic; they are not product benchmark evidence.
