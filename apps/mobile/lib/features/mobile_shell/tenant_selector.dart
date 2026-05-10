class TenantSelector {
  const TenantSelector(this.availableTenants);
  final List<String> availableTenants;

  String select(String? requested) {
    if (requested != null && availableTenants.contains(requested)) {
      return requested;
    }
    return availableTenants.first;
  }
}
