# Cloudfront distribution
#   Woah boy does cloudfront need a loooooot of configuration!
resource "aws_cloudfront_distribution" "www" {
  # Whether this distribution is ... enabled
  enabled = true

  # Domain names that will be pointing at this distribution,
  #   other than the auto-generated "______.cloudfront.net"
  aliases = ["${var.domain_name}"]

  # Default file to serve when requesting `/`
  default_root_object = "index.html"

  # Pricing tier - see https://aws.amazon.com/cloudfront/pricing/ for details
  # Basically just US / Canada - should be faster to provision
  #   PriceClass_100 takes about ~20m to geo-replicate
  #   PriceClass_All takes about ~45m to geo-replicate
  price_class = "PriceClass_100"

  # WWW (through www-proxy)
  origin {
    domain_name = module.www_proxy.invoke_url
    origin_id   = local.www_origin_id

    custom_origin_config {
      http_port              = 80
      https_port             = 443
      origin_protocol_policy = "https-only"
      origin_ssl_protocols   = ["TLSv1.2"]
    }
  }

  # API
  origin {
    domain_name = module.api.invoke_url
    origin_id   = local.api_origin_id

    custom_origin_config {
      http_port              = 80
      https_port             = 443
      origin_protocol_policy = "https-only"
      origin_ssl_protocols   = ["TLSv1.2"]
    }
  }

  # Cache config for WWW
  default_cache_behavior {
    allowed_methods        = ["HEAD", "GET", "OPTIONS"]
    cached_methods         = ["HEAD", "GET"]
    compress               = true
    viewer_protocol_policy = "redirect-to-https"
    target_origin_id       = local.www_origin_id

    # Cache retention (1 hour)
    default_ttl = 3600
    min_ttl     = 0
    max_ttl     = 3600

    forwarded_values {
      query_string = false

      cookies {
        forward = "none"
      }
    }
  }

  # Cache disabled for API
  ordered_cache_behavior {
    allowed_methods        = ["GET", "HEAD", "OPTIONS", "PUT", "POST", "PATCH", "DELETE"]
    cached_methods         = ["HEAD", "GET"]
    path_pattern           = "/api/*"
    compress               = true
    viewer_protocol_policy = "redirect-to-https"
    target_origin_id       = local.api_origin_id

    # Cache retention (disabled)
    default_ttl = 0
    min_ttl     = 0
    max_ttl     = 0

    forwarded_values {
      query_string = true

      cookies {
        forward = "all"
      }
    }
  }

  // Do not cache root
  ordered_cache_behavior {
    allowed_methods        = ["HEAD", "GET", "OPTIONS"]
    cached_methods         = ["HEAD", "GET"]
    path_pattern           = "/"
    compress               = true
    viewer_protocol_policy = "redirect-to-https"
    target_origin_id       = local.www_origin_id

    # Cache retention (disabled)
    default_ttl = 0
    min_ttl     = 0
    max_ttl     = 0

    forwarded_values {
      query_string = false

      cookies {
        forward = "none"
      }
    }
  }

  # Access restrictions
  # No restrictions, these are required fields
  restrictions {
    geo_restriction {
      restriction_type = "none"
    }
  }

  # AWS tags
  tags = {
    project     = var.project_id
    environment = var.environment_id
  }

  # HTTPS certificate (from ACM)
  viewer_certificate {
    acm_certificate_arn      = data.aws_acm_certificate.default.arn
    ssl_support_method       = "sni-only"
    minimum_protocol_version = "TLSv1.2_2018"
  }
}
