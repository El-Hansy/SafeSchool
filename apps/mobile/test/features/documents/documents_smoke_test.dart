import 'package:flutter_test/flutter_test.dart';
import '../../../lib/features/documents/documents.dart';

void main() {
  test('documents repository searches documents and certificates', () async {
    final repo = DocumentsRepository();

    expect(await repo.search('Amina'), containsAll(<String>['DOC-1', 'CERT-1']));
    expect(await repo.search(''), isEmpty);
  });
}
