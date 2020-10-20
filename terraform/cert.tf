# DATA
# Certificate from ACM - this is configured (once) by hand on the AWS website
data "aws_acm_certificate" "default" {
  provider = aws.us_east_1
  domain   = var.domain_name
}
