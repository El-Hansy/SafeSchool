class BlockedFeatureHandler {
  String messageFor(String reasonCode) => switch (reasonCode) {
        'FEATURE_DISABLED' => 'This feature is disabled for the school.',
        'ROLE_ACCESS_DENIED' => 'This role cannot access the feature.',
        _ => 'Access is not available.',
      };
}
