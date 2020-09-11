// acquire lock - success
// acquire new lock, different name - success
// acquire lock after original is released - success
// acquire lock twice at the same time - FAIL
// acquire lock with long execution time, but short acquire time - success (test with old client)
// lock action is null - throw
// action identifier is null - throw