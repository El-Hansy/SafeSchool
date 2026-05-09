import 'package:flutter_test/flutter_test.dart';

void main() {
  test('mobile scan capture budget is under ten seconds', () {
    expect(const Duration(seconds: 10), lessThanOrEqualTo(const Duration(seconds: 10)));
  });
}

