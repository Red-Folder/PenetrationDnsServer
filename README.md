# PenetrationDnsServer
Custom Dns server intended for Penetration Testing

WIP project intended to help with tracing a penetration vunerability identified by a 3rd party checker.

The checker worked by triggering a DNS query against a custom DNS entry (via an Oracle SQL Injection attack in this case).  The site in question had no Oracle, thus I'd expect that the vunderability was likely being triggered by a 3rd party (maybe a tracking script?).

To be able to investigate I created a custom Dns Server which could used to practice.

While now disabled, I had this linked into my Dns as a Nameserver for pentesting.red-folder.com.  This allowed me to perform an nslookup against {random}.pentesting.red-folder.com.  Any lookup would then be broadcast out to anyone registered to the service (via SignalR).

I build a very basic listener against the red-folder.com website.

Technically the solution all seemed to work.  

Never managed to recreate the actual vulnerability on the site in question - which some makes me question the 3rd paties tools & processes.
