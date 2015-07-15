# -*- mode: ruby -*-
# vi: set ft=ruby :

Vagrant.require_version ">= 1.6.0"
VAGRANTFILE_API_VERSION = "2" 

ENV['VAGRANT_DEFAULT_PROVIDER'] = 'docker' 

Vagrant.configure(2) do |config|
  config.vm.define "neo4j"
  config.vm.synced_folder ".", "/vagrant", disabled: true 
  config.vm.provider "docker" do |d| 
    d.name = "neo4j"
    d.image = "tpires/neo4j"
    d.ports = ['7474:7474']
		d.vagrant_vagrantfile = "./docker-host/Vagrantfile"
  end  
end