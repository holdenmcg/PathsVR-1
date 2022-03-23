
# ruleid:dockerfile-from-should-always-pin
FROM ruby

# ok (correctly pinned)
FROM ruby@sha256:e806e97b4a00c6b00f8baf36331f8798ab2d60b7d49f43a79577f7e7e3ab2623

# ruleid:dockerfile-from-should-always-pin
FROM ruby as base

# ok (correctly pinned)
FROM ruby@sha256:e806e97b4a00c6b00f8baf36331f8798ab2d60b7d49f43a79577f7e7e3ab2623 as base

# ruleid:dockerfile-from-should-always-pin
FROM ruby:latest 

# ok (correctly pinned)
FROM ruby:latest@sha256:e806e97b4a00c6b00f8baf36331f8798ab2d60b7d49f43a79577f7e7e3ab2623 

# ruleid:dockerfile-from-should-always-pin
FROM ruby:latest as base

# ok (correctly pinned)
FROM ruby:latest@sha256:e806e97b4a00c6b00f8baf36331f8798ab2d60b7d49f43a79577f7e7e3ab2623 as base

# ruleid:dockerfile-from-should-always-pin
FROM ruby:v9.9.9

# ok (correctly pinned)
FROM ruby:v9.9.9@sha256:e806e97b4a00c6b00f8baf36331f8798ab2d60b7d49f43a79577f7e7e3ab2623

# ruleid:dockerfile-from-should-always-pin
FROM ruby:v9.9.9 as base

# ok (correctly pinned)
FROM ruby:v9.9.9@sha256:e806e97b4a00c6b00f8baf36331f8798ab2d60b7d49f43a79577f7e7e3ab2623 as base
