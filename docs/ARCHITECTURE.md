# Architecture

The .NET 8 solution separates domain records (`Core`) from Windows fact gathering (`Hardware`), local library/profile behavior (`Optimization`), observed-sample analysis (`Benchmark`), optional streaming integration (`Streaming`), path and archive safety (`Security`), and the WinUI 3 desktop shell (`App`). The UI calls local services directly; no backend server is required.

Data flow: user action → local service → Windows APIs or `%LOCALAPPDATA%\AstraGPU` → explicit status in UI. No telemetry endpoint exists. Streaming has no listener or protocol implementation in this MVP. Benchmark capture adapters are also not implemented; analysis accepts actual samples when an adapter is added.

## Next implementation increments

1. Add documented Windows-native GPU inventory and sensor provider interfaces with consent and unavailable states.
2. Add library editing, categorized profile actions, and transactional backup preview/rollback.
3. Integrate an established frame-time capture source after licensing, security, and reproducibility review.
4. Design authenticated, opt-in local-network streaming with explicit pairing and no internet relay.
