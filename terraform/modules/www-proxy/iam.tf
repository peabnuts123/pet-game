# Execution role for www-proxy Lambda
resource "aws_iam_role" "lambda" {
  name        = "${var.project_id}_${var.environment_id}_www-proxy_lambda"
  description = "Allow Lambda access to create logs and read parameter store"

  # Allow Lambda AWS service to assume this role
  assume_role_policy = <<-POLICY
    {
      "Version": "2012-10-17",
      "Statement": [
        {
          "Effect": "Allow",
          "Action": "sts:AssumeRole",
          "Principal": {
            "Service": "lambda.amazonaws.com"
          },
          "Sid": ""
        }
      ]
    }
    POLICY
}

resource "aws_iam_policy" "lambda" {
  name        = "${var.project_id}_${var.environment_id}_www-proxy_lambda"
  description = "Allow Lambda access to create logs read parameter store"

  policy = <<-POLICY
  {
    "Version": "2012-10-17",
    "Statement": [
      {
        "Effect": "Allow",
        "Action": [
          "ssm:GetParametersByPath"
        ],
        "Resource": [
          "arn:aws:ssm:${var.aws_region}:${var.aws_account_id}:parameter${local.parameter_store_prefix}/*"
        ]
      },
      {
        "Effect": "Allow",
        "Action": [
          "s3:GetObject",
          "s3:ListBucket"
        ],
        "Resource": [
          "arn:aws:s3:::${var.www_bucket_name}/*",
          "arn:aws:s3:::${var.www_bucket_name}"
        ]
      },
      {
        "Effect": "Allow",
        "Action": [
          "logs:CreateLogStream",
          "logs:PutLogEvents"
        ],
        "Resource": [
          "arn:aws:logs:${var.aws_region}:${var.aws_account_id}:log-group:${local.lambda_log_group_name}:*",
          "arn:aws:logs:${var.aws_region}:${var.aws_account_id}:log-group:${local.lambda_log_group_name}:log-stream:*"
        ]
      }
    ]
  }
  POLICY
}

resource "aws_iam_role_policy_attachment" "lambda" {
  role       = aws_iam_role.lambda.name
  policy_arn = aws_iam_policy.lambda.arn
}
