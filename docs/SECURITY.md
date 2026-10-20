# Security

- Run as a standard user. There is no elevation path or privileged service.
- Do not change anti-virus, firewall, anti-cheat, drivers, clocks, voltages, registry policy, or Windows services.
- Validate paths before use. Library additions accept only existing `.exe` files. Archive restore rejects paths escaping the chosen directory.
- Backups can overwrite files inside the selected destination; users should choose a dedicated directory. Review/preview and atomic rollback are future work.
- Streaming is not implemented. Any future transport must be opt-in, paired/authenticated, LAN-only by default, and must not expose a listener without user action.
- Report vulnerabilities privately to the repository maintainer; do not include personal machine data in issue reports.
