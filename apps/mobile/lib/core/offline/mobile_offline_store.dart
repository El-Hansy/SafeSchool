class MobileOfflineStore {
  final Map<String, OfflineMobileAction> _actions = {};

  bool get isEmpty => _actions.isEmpty;
  List<OfflineMobileAction> get actions => _actions.values.toList(growable: false);

  OfflineSyncDecision queue(OfflineMobileAction action) {
    if (!action.offlineAllowed) {
      return const OfflineSyncDecision('rejected', 'Offline not supported');
    }
    if (_actions.containsKey(action.clientActionId)) {
      return const OfflineSyncDecision('duplicate', 'Duplicate ignored');
    }
    _actions[action.clientActionId] = action;
    return const OfflineSyncDecision('queued', 'Offline action queued');
  }
}

class OfflineMobileAction {
  const OfflineMobileAction({
    required this.clientActionId,
    required this.sourceFeature,
    required this.actorId,
    required this.offlineAllowed,
  });

  final String clientActionId;
  final String sourceFeature;
  final String actorId;
  final bool offlineAllowed;
}

class OfflineSyncDecision {
  const OfflineSyncDecision(this.status, this.message);
  final String status;
  final String message;
}
