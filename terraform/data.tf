# Dummy archive
# This is used for provisioning Lambda functions without uploading any code
data "archive_file" "empty" {
  type        = "zip"
  output_path = "${path.module}/tmp/empty.zip"

  source {
    content  = "hello"
    filename = "dummy.txt"
  }
}
